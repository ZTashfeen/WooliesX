Feature: WooliesX QA Code Challenge

@mytag
Scenario: I am able to navigate to url and place order successfully
	Given I navigate to url
	And I add items to cart and sign in successfully
	When I place order
	Then Order is successfully placed