# Hacker-News-Demo-Hazel
Hacker News Angular App With .NET Backed


## How to Start

Browse to the solution folder and open the sln file in the latest version of Visual Studio 2022. Any Visual Studio features needed should get prompted for installation when the solution opens.

1. Right-click the following project and click "Set as Start-up Project":

```Hazel.Assessment.Web.UI.Server```

2. Open a command window to the path containing the Hazel.Assessment.Web.UI.Client project.


- Run the following, if you do not have Angular 17 installed globally, or do not want to install Angular 17 globally:

```npm install @angular/cli@17.3.0```

- Run the following command if you are unsure of what version is installed on your machine:

```ng version```

- Run the following (if you have Angular 17 installed globally):

```npm install```

3. Press F5 or the green play button at the top of Visual Studio. The Server should begin querying news stories, then the ng Angular window should open and start the web page. Using a browser, nagivate to the webpage provided (default is https://127.0.0.1:50109)

## Troubleshooing

1. If you get an error like the following

	...esproj' failed: Value cannot be null

	Try right-clicking the Hazel.Assessment.Web.UI.Server project, select Debug and Start New Instance. The backend should start and provide a local url to open in a browser. The default is:

	```https://127.0.0.1:50109```

## License

[MIT](https://choosealicense.com/licenses/mit/)
