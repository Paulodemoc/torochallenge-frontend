import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Helpers } from '../helpers/helpers';
import { BaseService } from './base.service';

@Injectable({
  providedIn: 'root'
})
export class StocksService extends BaseService {
  myApiUrl: string;

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json; charset=utf-8'
    })
  };

  constructor(private http: HttpClient, helper: Helpers) { super(helper); }

  buyStock({ stock, quantity }: { stock: string; quantity: number; }): Observable<any> {
    return this.http.put(this.pathAPI + 'stocks/buy/' + this.helper.getUserId(), {
      stockcode: stock,
      ammount: quantity
    }, super.header());
  }

  sellStock({ stock, quantity }: { stock: string; quantity: number; }): Observable<any> {
    return this.http.put(this.pathAPI + 'stocks/sell/' + this.helper.getUserId(), {
      stockcode: stock,
      ammount: quantity
    }, super.header());
  }
}
