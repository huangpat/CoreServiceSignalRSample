import { Component, Inject, ChangeDetectorRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public forecasts: WeatherForecast[];
  public games: string[] = [];
  private http: HttpClient;
  private baseUrl: string;


  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
    http.get<WeatherForecast[]>(baseUrl + 'api/weatherforecast' + "/Get").subscribe(result => {
      this.forecasts = result;
    }, error => console.error(error));

    this.refreshGameList();
  }

  private refreshGameList() {
    this.http.get<string[]>(this.baseUrl + 'api/weatherforecast' + '/GetGames').subscribe(result => {
      console.log("refreshGameList", result);
      this.games = result;
      console.log("refreshGameList games", this.games);
      
    }, error => console.error(error));
  }

  public addGame() {


    let count: number = this.games.length;

    this.http.post<boolean>(this.baseUrl + 'api/weatherforecast' + '/AddGame', { index: this.games.length + 1 })
      .subscribe(data => { console.log("addGame return " + data); });

    this.refreshGameList();    
  }
}

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
