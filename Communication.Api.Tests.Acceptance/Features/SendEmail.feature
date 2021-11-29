Feature: SendEmail
	In order to keep users updated
	I want to send email to user
	
@mytag
Scenario: Send instant email
	Given following message values
		| To                        | Text                  |
		| integration.test@mail7.io | Email acceptance test |
	When the client posts the inputs to send endpoint
	Then the result should be true
	And client receives and Email with subject 'Test'