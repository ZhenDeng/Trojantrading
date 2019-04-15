import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators/catchError';
import { HttpErrorResponse, HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { ShoppingCart } from '../models/shoppingCart';

@Injectable()
export class ShoppingCartServiceService {

  base_url: string = "";

  constructor(private http: HttpClient) { }

  GetShoppingCart(): Observable<ShoppingCart> {
    return this.http.get(this.base_url + "/authenticate")
      .pipe(catchError(this.handleError));
  }

  private handleError(error: HttpErrorResponse) {
    console.error('server error:', error);
    if (error.error instanceof ErrorEvent) {
      let errMessage = '';
      try {
        errMessage = error.error.message;
      } catch (err) {
        errMessage = error.statusText;
      }
      return Observable.throw(errMessage);
    }
    return Observable.throw(error.error || 'ASP.NET Core server error');
  }
}
