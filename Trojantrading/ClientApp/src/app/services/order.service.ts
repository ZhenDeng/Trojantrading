import { Injectable } from '@angular/core';
import { ShoppingCart } from '../models/shoppingCart';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/ApiResponse';
import { catchError } from 'rxjs/operators';
import { HttpErrorResponse, HttpClient } from '@angular/common/http';
import { Order } from '../models/order';

@Injectable()
export class OrderService {

  base_url: string = "api/Order";

  constructor(private http: HttpClient) { }

  AddOrder(cart: ShoppingCart): Observable<ApiResponse>{
    return this.http.post(this.base_url + "/AddOrder", cart)
      .pipe(catchError(this.handleError));
  }

  GetOrderWithUser(userId: number): Observable<Order[]>{
    return this.http.get(this.base_url + "/GetOrderWithUser?userId=" + userId)
      .pipe(catchError(this.handleError));
  }

  getOrdersByUserID(id: string, dateFrom: string, dateTo: string): Observable<any> {
    return this.http.get(this.base_url + '/GetOrdersByUserID?userId=' + id + '&dateFrom=' + dateFrom + '&dateTo=' + dateTo)
    .pipe(catchError(this.handleError));
  }

  exportOrdersToExcel(id: string, dateFrom: string, dateTo: string): Observable<any> {
    return this.http.get(this.base_url + '/ExportOrdersToExcel?userId=' + id + '&dateFrom=' + dateFrom + '&dateTo=' + dateTo)
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