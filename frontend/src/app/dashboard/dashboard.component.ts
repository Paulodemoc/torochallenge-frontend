import { Stock } from './../models/stock';
import { DashboardService } from './../services/dashboard.service';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { WebSocketSubject } from 'rxjs/webSocket';
import { ChartOptions, ChartType, ChartDataSets } from 'chart.js';
import { Label, Color } from 'ng2-charts';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  myWebSocket$: WebSocketSubject<any>;
  stocks: Stock[];

  chartOptions: ChartOptions = {
    responsive: true,
  };
  chartLabels: Label[] = [];
  chartType: ChartType = 'line';
  chartLegend = false;
  chartPlugins = [];

  chartData: ChartDataSets[] = [
    { data: [], label: 'Quotes' }
  ];

  chartColors: Color[] = [
    {
      borderColor: 'black',
      backgroundColor: 'rgba(255,255,255,0.28)',
    },
  ];

  constructor(private dashboardService: DashboardService) { }

  ngOnInit() {
    this.stocks = [];
    this.myWebSocket$ = this.dashboardService.getStocks();

    this.myWebSocket$.subscribe(
        msg => {
          Object.keys(msg).forEach((key: string) => {
            if(key !== 'timestamp'){
              let stockIndex: number = this.stocks.findIndex(s => s.StockCode === key);
              if (stockIndex === -1){
                const stock: Stock = new Stock();
                stock.StockCode = key;
                stockIndex = this.stocks.push(stock) - 1;
              }

              this.stocks[stockIndex].Value = msg[key];
              this.stocks[stockIndex].Timestamp = msg.timestamp;

              this.chartLabels = this.stocks.map((stock, index, stocks) => {
                return stock.StockCode;
              });

              this.chartData[0].data = this.stocks.map((stock, index, stocks) => {
                return stock.Value;
              });
            }
          });
        },
        // Called whenever there is a message from the server
        err => console.log(err),
        // Called if WebSocket API signals some kind of error
        () => console.log('complete')
        // Called when connection is closed (for whatever reason)
    );
  }

  clicked() {
    console.log(this.myWebSocket$);
  }
}
