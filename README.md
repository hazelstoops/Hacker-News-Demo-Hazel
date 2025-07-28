# HazelAssessment

This is a demonstration of an Angular 17 Client using a NET9.0 Server Backend.

## How to Start

Browse to the solution folder and open the sln file using the latest version of Visual Studio 2022. Any Visual Studio features needed should get prompted for installation when the solution opens.

1. Right-click the following project and click "Set as Start-up Project":

```Hazel.Assessment.Web.UI.Server```

2. Press F5 or the green play button at the top of Visual Studio. The Server should begin querying news stories, then the ng Angular window should open and start the web page. Using a browser, nagivate to the webpage provided (default is https://127.0.0.1:50109).

## Unit Tests

1. Backend and Server Controller Tests can be run using the XUnit extensions in Visual Studio.
2. Frontend (Angular) tests are using Karma/Jasmine can be run by using a command prompt to navigate the following project folder
   ```Hazel.Assessment.Web.UI/hazel.assessment.web.ui.client```
   And running the following command:
   ```ng test```

## License

[MIT](https://choosealicense.com/licenses/mit/)
