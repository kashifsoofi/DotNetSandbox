﻿Feature: Messages
	In order to keep users updated
	I want to send email to user
	
@mytag
Scenario: Send instant email
	Given I have entered 50 into the calculator
	And I have entered 70 into the calculator
	When I press add
	Then the result should be 120 on the screen
