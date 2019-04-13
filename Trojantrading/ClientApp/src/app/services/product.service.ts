import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Product } from '../models/Product';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ProductService {

  base_url: string = "api/Product";

  constructor(
    private http: HttpClient
  ) { }

  getAllProducts(): Observable<Product[]> {
    console.log('service');
    return this.http.get(this.base_url + "/GetAllProducts")
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
