import { Stock } from './../models/stock';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { retry, catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import {webSocket, WebSocketSubject} from 'rxjs/webSocket';
import { BaseService } from './base.service';
import { Helpers } from '../helpers/helpers';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class UsersService extends BaseService {
  public errorMessage: string;

  constructor(private http: HttpClient, helper: Helpers) { super(helper); }

  getUserData(): Observable<User> {
    return this.http.get<User>(this.pathAPI + 'users/getUserData/' + this.helper.getUserId(), super.header()).pipe(
      catchError(super.handleError)
    );
  }
}
