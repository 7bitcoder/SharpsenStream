import { Injectable } from '@angular/core';
import { EMPTY, Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { User, UserClient, UserCreditials } from '../api/Api';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  user: User = null;
  isLoggedIn: boolean = null;
  constructor(private user$: UserClient) {}
  
  tryFetchUser(): Observable<boolean> {
    return this.user$.get().pipe(
      catchError( error => this.handleError(error) ),
      map( val => this.mapResult(val) )
    )
  }
  
  login(userName: string, password: string): Observable<boolean> {
    const creditials = new UserCreditials({
      userName: userName,
      password: password
    })
    return this.user$.login(creditials).pipe(
      catchError( error => this.handleError(error) ),
      map( val => this.mapResult(val) )
    )
  }

  logout(userName: string, password: string): Observable<boolean> {
    if( this.isLogged() ) {
      return this.user$.logout(this.user);
    }
    return of(false);
  }

  isLogged(): boolean { return this.isLoggedIn; }
  getUser(): User { return this.user; }

  private handleError(error: any): Observable<User> {
    debugger;
    this.isLoggedIn = false
    return of(null);
  }

  private mapResult(user: User): boolean {
    debugger;
    if( !!user ) {
      this.user = user;
      this.isLoggedIn = true;
    } else {
      this.isLoggedIn = false;
    }
    return this.isLoggedIn;
  }
}
