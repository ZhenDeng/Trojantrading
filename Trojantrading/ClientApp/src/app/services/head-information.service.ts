import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { HeadInformation } from '../models/header-info';
import { catchError } from 'rxjs/operators';
import { ApiResponse } from '../models/ApiResponse';

@Injectable()
export class HeadInformationService {

  base_url: string = "api/HeadInfomation";

  private messageSource = new BehaviorSubject<Number>(0);
  currentMessage = this.messageSource.asObservable();

  constructor(private http: HttpClient) { }

  changeMessage(message: Number) {
    this.messageSource.next(message)
  }

  GetHeadInformation(): Observable<HeadInformation[]> {
    return this.http.get(this.base_url + "/GetHeadInformation")
      .pipe(catchError(this.handleError));
  }

  AddHeader(headInformation: HeadInformation): Observable<ApiResponse> {
    return this.http.post(this.base_url + "/AddHeader", headInformation)
      .pipe(catchError(this.handleError));
  }

  UpdateHeadInfomation(headInformation: HeadInformation): Observable<ApiResponse> {
    return this.http.post(this.base_url + "/UpdateHeadInfomation", headInformation)
      .pipe(catchError(this.handleError));
  }

  DeleteHeadInfomation(headInformation: HeadInformation): Observable<ApiResponse> {
    return this.http.post(this.base_url + "/DeleteHeadInfomation", headInformation)
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
