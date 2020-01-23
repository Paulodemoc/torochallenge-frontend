import { environment } from 'src/environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { BaseService } from './base.service';
import { Token } from '../models/token';
import { Helpers } from '../helpers/helpers';

@Injectable()
export class TokenService extends BaseService {
  public errorMessage: string;

  constructor(private http: HttpClient, helper: Helpers) { super(helper); }

  auth(data: any): any {
    const body = JSON.stringify(data);
    return this.getToken(body);
  }

  private getToken(body: any): Observable<any> {
    return this.http.post<any>(this.pathAPI + 'users/authenticate', body, super.header()).pipe(
        catchError(super.handleError)
      );
  }
}
