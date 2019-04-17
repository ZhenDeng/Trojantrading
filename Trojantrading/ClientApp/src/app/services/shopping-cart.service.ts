import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/ApiResponse';
import { catchError } from 'rxjs/operators';
import { ShoppingCart } from '../models/shoppingCart';
import { ShoppingItem } from '../models/shoppingItem';

@Injectable()
export class ShoppingCartService {

  base_url: string = "api/ShoppingCart";

  constructor(private http: HttpClient) { }

  AddShoppingCart(userId: number): Observable<ApiResponse>{
    return this.http.get(this.base_url + "/GetUserWithAddress?userId=" + userId)
      .pipe(catchError(this.handleError));
  }

  GetShoppingCart(userId: number): Observable<ShoppingCart>{
    return this.http.get(this.base_url + "/GetShoppingCart?userId=" + userId)
      .pipe(catchError(this.handleError));
  }

  UpdateShoppingCart(userId: number, shoppingCartItem: ShoppingItem): Observable<ApiResponse>{
    return this.http.post(this.base_url + "/UpdateShoppingCart?userId=" + userId, shoppingCartItem)
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
