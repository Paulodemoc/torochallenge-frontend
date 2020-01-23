import { Stock } from './../models/stock';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { retry, catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import {webSocket, WebSocketSubject} from 'rxjs/webSocket';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {

  myApiUrl: string;
  stocksWebSocket: string;
  myWebSocket: WebSocketSubject<Stock>;

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json; charset=utf-8'
    })
  };

  constructor(private http: HttpClient) {
      this.myApiUrl = environment.apiUrl;
      this.stocksWebSocket = environment.stocksSocket;

      this.myWebSocket = webSocket(this.stocksWebSocket);
  }

  getStocks(): WebSocketSubject<Stock> {
    return this.myWebSocket;
  }
}
