import { StocksService } from './../services/stocks.service';
import { UsersService } from './../services/users.service';
import { Stock } from './../models/stock';
import { DashboardService } from './../services/dashboard.service';
import { Component, OnInit, Input, AfterViewInit } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { WebSocketSubject } from 'rxjs/webSocket';
import { ChartOptions, ChartType, ChartDataSets } from 'chart.js';
import { Label, Color } from 'ng2-charts';
import { Helpers } from '../helpers/helpers';
import { startWith, delay } from 'rxjs/operators';
import { User } from '../models/user';
import { ToastrService } from 'ngx-toastr';
import { Account } from '../models/account';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit, AfterViewInit {
  myWebSocket$: WebSocketSubject<any>;
  stocks: Stock[];
  userData: User;
  subscription: Subscription;
  authentication: boolean;
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

  constructor(private dashboardService: DashboardService, private helpers: Helpers,
              private userService: UsersService, private stocksService: StocksService,
              private toastr: ToastrService) {
                this.userData = new User();
               }

  ngOnInit() {
    this.stocks = [
      {StockCode:"ABC", Value:5, Ammount: 0, AmmountToBuy: 0, AmmountToSell: 0, Timestamp: null},
      {StockCode:"DEF", Value:10, Ammount: 0, AmmountToBuy: 0, AmmountToSell: 0, Timestamp: null},
      {StockCode:"GHI", Value:15, Ammount: 0, AmmountToBuy: 0, AmmountToSell: 0, Timestamp: null}
    ];
    this.myWebSocket$ = this.dashboardService.getStocks();

    this.myWebSocket$.subscribe(
        msg => {
          Object.keys(msg).forEach((key: string) => {
            if (key !== 'timestamp') {
              this.updateStockValue(key, msg);
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

  ngAfterViewInit() {
    this.subscription = this.helpers.isAuthenticationChanged().pipe(
      startWith(this.helpers.isAuthenticated()),
      delay(0)).subscribe((value: boolean) => {
        this.authentication = value;
        if (value) {
          this.getUserData();
        }
      });
  }

  OnDestroy() {
    this.subscription.unsubscribe();
  }

  private getUserData() {
    this.userService.getUserData().subscribe(user => {
      this.userData = user;
      if(this.userData == null) {
        this.userData = new User();
      }
      if(this.userData.Portfolio.length > 0){
        this.stocks = this.stocks.map((stock, index, portfolio) => {
          stock.Ammount = 0;
          return stock;
        })
        this.userData.Portfolio.forEach((stock, index, portfolio) => {
          if(this.stocks.find(x => x.StockCode === stock.StockCode)){
            this.stocks.find(x => x.StockCode === stock.StockCode).Ammount = stock.Ammount;
          }
        });
      }

      this.sortStocks();
    });
  }

  private sortStocks() {
    this.stocks.sort((a, b) => {
      if (a.Ammount > b.Ammount) {
        return -1;
      }
      if (a.Ammount < b.Ammount) {
        return 1;
      }
      return 0;
    });
  }

  private updateStockValue(key: string, msg: any) {
    let stockIndex: number = this.stocks.findIndex(s => s.StockCode === key);
    if (stockIndex === -1) {
      const stock: Stock = new Stock();
      stock.Ammount = 0;
      stock.StockCode = key;
      stockIndex = this.stocks.push(stock) - 1;
    }
    this.stocks[stockIndex].Value = msg[key];
    this.stocks[stockIndex].Timestamp = msg.timestamp;

    if (this.userData != null && this.userData.Portfolio != null
      && this.userData.Portfolio.find(x => x.StockCode === this.stocks[stockIndex].StockCode) != null) {
      this.stocks[stockIndex].Ammount = this.userData.Portfolio.find(x => x.StockCode === this.stocks[stockIndex].StockCode).Ammount;
    }

    this.chartLabels = this.stocks.map((stock, index, stocks) => {
      return stock.StockCode;
    });
    this.chartData[0].data = this.stocks.map((stock, index, stocks) => {
      return stock.Value;
    });

    this.sortStocks();
  }

  buy(stock: string, ammount: number) {
    if (ammount === undefined) {
      this.toastr.warning('Please inform the ammount you want to buy', 'Attention');
      return;
    }
    this.stocksService.buyStock({ stock, quantity: ammount }).subscribe((data) => {
      this.toastr.success(ammount + ' stocks of ' + stock + ' have been bought');
      this.getUserData();
    }, (error) => {
      console.log(error);
      this.toastr.error(error.error, error.status);
    });
  }

  sell(stock: string, ammount: number) {
    if (ammount === undefined) {
      this.toastr.warning('Please inform the ammount you want to sell', 'Attention');
      return;
    }
    this.stocksService.sellStock({ stock, quantity: ammount }).subscribe((data) => {
      this.toastr.success(ammount + ' stocks of ' + stock + ' have been sold');
      this.getUserData();
    }, (error) => {
      this.toastr.error(error.error, error.status);
    });
  }
}
