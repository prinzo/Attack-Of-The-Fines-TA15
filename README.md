# Attack-Of-The-Fines-TA15

## Entelect Technology Accelerator Project 2015

### Team Members:
- Prinay Panday
- Kurt Vining
- Kristina Georgieva
- Amrit Purshotam

### Technology Used:
- Angular JS
- C# Web API
- .Net 4.5
- MongoDB
- Slack

### Description:

This project is intended to keep track of Entelect employees' fines.
It contains a web application and a chat bot for Slack.

### Setup Instructions:

#### To run the web project:
- Open a command prompt to the web project directory
- Install the npm packages listed in "Gulp Setup.txt"
- Run the command "gulp watch" to build the project
- Open the application in your browser

#### To run the slack bot:
- Check out the FineBot solution folder
- Open the project in Visual Studio and build the solution
- Run the BotRunner project
- Open slack and the bot should be online

#### To view Web API docs using swagger:
- Run the Web Api project from Visual Studio
- Append "/swagger" to the URL

NOTE: If git complains about a path that is too long when checking out, use the following to enable long paths:

git config core.longpaths true
