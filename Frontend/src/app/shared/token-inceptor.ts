import {
  HttpRequest,
  HttpHandler,
  HttpEvent
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable()
export class TokenInterceptor {

/**
 * Creates an instance of TokenInterceptor.
 * @param {OidcSecurityService} auth
 * @memberof TokenInterceptor
 */
constructor() {}

/**
 * Intercept all HTTP request to add JWT token to Headers
 * @param {HttpRequest<any>} request
 * @param {HttpHandler} next
 * @returns {Observable<HttpEvent<any>>}
 * @memberof TokenInterceptor
 */
intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

  const token = localStorage.getItem("token");
  if (token) {
    request = request.clone({
        setHeaders: {
            Authorization: `Bearer ${token}`
        }
    });
  }
    return next.handle(request);
  }
}