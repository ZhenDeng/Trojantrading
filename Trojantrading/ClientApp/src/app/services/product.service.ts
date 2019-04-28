import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Product } from '../models/Product';
import { catchError } from 'rxjs/operators';
import { Order } from '../models/order';

@Injectable()
export class ProductService {

  product_url: string = "api/Product";
  order_url: string = "api/Order";

  constructor(
    private http: HttpClient
  ) { }

  getAllProducts(): Observable<Product[]> {
    return this.http.get(this.product_url + "/GetAllProducts")
    .pipe(catchError(this.handleError));
  }

  getOrdersByUserID(id: string): Observable<Order[]> {
    return this.http.get(this.order_url + '/GetOrdersByUserID?userId=' + id)
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
