using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using TechTalk.SpecFlow;

namespace WooliesSpecFlow.Steps
{
    [Binding]
    public class CircuitsApiSteps
    {
        private const string APIEndpoint = "http://ergast.com/api/f1?limit=1037&offset=0";
        WebRequest request;
        WebResponse response;
        XDocument xDoc;
        List<Race> Races;
        
        [Given(@"I want to know the number of Formula One races in (.*)")]
        public void GivenIWantToKnowTheNumberOfFormulaOneRacesIn(int Season)
        {
            Console.WriteLine("Number of Races in " + Season + " is " + NumberofRaces(Season));
        }

        [When(@"I retrieve the circuit list for that season")]
        public void WhenIRetrieveTheCircuitListForThatSeason()
        {
            Console.WriteLine("======Circuit List======");
            Console.WriteLine("========================");
            foreach (var circuit in Races)
            {
                Console.WriteLine(circuit.Circuit);
            }
            Console.WriteLine("========================");
        }

        [Then(@"there should be (.*) circuits in the list returned")]
        public void ThenThereShouldBeCircuitsInTheListReturned(int CircuitNumbers)
        {
            if(Races.Count == CircuitNumbers)
            {
                Console.WriteLine("Circuit Numbers Returned in list " + Races.Count + " are equal");
            }
        }

        public int NumberofRaces(int Season)
        {
            var numberOfRace = 0;
            Races = new List<Race>();
            
            request = WebRequest.Create(APIEndpoint);
            response = request.GetResponse();
            using (Stream responseStream = response.GetResponseStream())
            {
                xDoc = XDocument.Load(responseStream);
            }
            var nsManager = new XmlNamespaceManager(new NameTable());
            nsManager.AddNamespace("t", xDoc.Root.GetDefaultNamespace().NamespaceName);
            int season=0;
            foreach (var element in xDoc.Descendants())
            {
                
                if (element.Name.LocalName == "Race")
                {
                    season = Int32.Parse(element.FirstAttribute.Value);
                    if (season == Season)
                    {                    
                        var nodes = element.Nodes();
                        numberOfRace++; 
                    }
                }
                if(element.Name.LocalName == "CircuitName" && season == Season)   
                {
                    Races.Add(new Race() { Circuit = element.Value });
                }
            }
            return numberOfRace;
        }


    }
}
