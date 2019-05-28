import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { ApiResponse } from '../models/ApiResponse';
import { catchError } from 'rxjs/operators';
import { ShoppingCart } from '../models/shoppingCart';
import { ShoppingItem } from '../models/shoppingItem';

@Injectable()
export class ShoppingCartService {

  base_url: string = "api/ShoppingCart";
  shoppingItemLength: number;

  private shoppingItemLengthSource = new BehaviorSubject<number>(this.shoppingItemLength);
  currentShoppingItemLength = this.shoppingItemLengthSource.asObservable();

  constructor(private http: HttpClient) { }

  MonitorShoppingItemLength(shoppingItemsLength: number): void{
    this.shoppingItemLengthSource.next(shoppingItemsLength);
  }

  AddShoppingCart(userId: number): Observable<ApiResponse>{
    return this.http.get(this.base_url + "/AddShoppingCart?userId=" + userId)
      .pipe(catchError(this.handleError));
  }

  GetShoppingCart(userId: number): Observable<ShoppingCart>{
    return this.http.get(this.base_url + "/GetShoppingCart?userId=" + userId)
      .pipe(catchError(this.handleError));
  }

  GetCartInIdWithShoppingItems(shoppingCartId: number): Observable<ShoppingCart>{
    return this.http.get(this.base_url + "/GetCartInIdWithShoppingItems?shoppingCartId=" + shoppingCartId)
      .pipe(catchError(this.handleError));
  }

  UpdateShoppingCart(userId: number, shoppingCartItem: ShoppingItem): Observable<ApiResponse>{
    return this.http.post(this.base_url + "/UpdateShoppingCart?userId=" + userId, shoppingCartItem)
      .pipe(catchError(this.handleError));
  }

  UpdateShoppingCartPaymentMethod(userId: number, selectedPayment: string): Observable<ApiResponse>{
    return this.http.get(this.base_url + "/UpdateShoppingCartPaymentMethod?userId=" + userId + "&selectedPayment=" + selectedPayment)
      .pipe(catchError(this.handleError));
  }

  DeleteShoppingItem(shoppingItemId: number): Observable<ApiResponse>{
    return this.http.delete(this.base_url + "/deleteShoppingItem?shoppingItemId=" + shoppingItemId)
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
