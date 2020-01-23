import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { Helpers } from '../helpers/helpers';
import { environment } from 'src/environments/environment';

@Injectable()
export class BaseService {
  public pathAPI = environment.apiUrl;
  constructor(public helper: Helpers) { }

  public extractData(res: Response) {
        const body = res.json();
        return body || {};
      }

      public handleError(error: Response | any) {
        // In a real-world app, we might use a remote logging infrastructure
        let errMsg: string;
        if (error instanceof Response) {
          const body = error.json() || '';
          const err = body || JSON.stringify(body);
          errMsg = `${error.status} - ${error.statusText || ''} ${err}`;
        } else {
          errMsg = error.message ? error.message : error.toString();
        }
        console.error(errMsg);
        return Observable.throw(errMsg);
    }

    public header() {
        let header = new HttpHeaders({ 'Content-Type': 'application/json' });
        if (this.helper.isAuthenticated()) {
          header = header.append('Authorization', 'Bearer ' + this.helper.getToken());
        }

        return { headers: header };
      }

      public setToken(data: any) {
        this.helper.setToken(data);
      }

      public failToken(error: Response | any) {
        this.helper.failToken();
        return this.handleError(Response);
      }
 }
