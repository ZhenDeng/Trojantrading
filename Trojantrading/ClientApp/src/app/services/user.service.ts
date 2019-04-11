import { Injectable } from '@angular/core';
import { User } from '../models/user';
import { HttpErrorResponse, HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { UserResponse } from '../models/ApiResponse';

@Injectable()
export class UserService {

  selectedUser: User;
  base_url: string = "api/User";

  constructor(private http: HttpClient) { }

  userAuthentication(user: any): Observable<UserResponse> {
    return this.http.post(this.base_url + "/authenticate", user, {headers: new HttpHeaders({'Content-Type':'application/json', 'No-Auth': 'True'})})
      .pipe(catchError(this.handleError));
  }
  
  PasswordRecover(email: string): Observable<UserResponse> {
    return this.http.get(this.base_url + "/PasswordRecover?email=" + email)
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