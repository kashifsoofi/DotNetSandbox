Feature: Messages
	In order to keep users updated
	I want to send email to user
	
@mytag
Scenario: Send instant email
	Given following message values
		| To   | integration.test@mailtrap.io |
		| Text | Email acceptance test        |
	When the client posts the inputs to send endpoint
	Then an Ok status should be returned
