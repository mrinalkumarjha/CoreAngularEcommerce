import { Injectable } from '@angular/core';
import {environment} from '../../environments/environment';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import { BehaviorSubject, from, of, ReplaySubject } from 'rxjs';
import {map} from 'rxjs/operators';
import {Router} from '@angular/router';
import { IUser } from '../shared/models/user';
import { IAddress } from '../shared/models/address';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl: string = environment.apiUrl;

  // we have replaced BehavoiurSubject to REplaySubject as BehavoiurSubject was emiting initial value immidiately
  // Which was causing auth guard to redirect to login even user is login. as it is getting null initially due to behaviourSub
  // Now Auth guard will wait for value as replaysubject does not emit initial value.
  private currentUserSource = new ReplaySubject<IUser>(1);

  currentUser$ = this.currentUserSource.asObservable();
  private isAdminSource = new ReplaySubject<boolean>(1);
  isAdmin$ = this.isAdminSource.asObservable();

  constructor(private http: HttpClient, private router: Router) { }


  // It will used to load user if token available inside localstorage. Used to persist user on refrersh page
  loadCurrentUser(token: string): any{

    if (token === null) {
      this.currentUserSource.next(null);
      return of(null); // since while subscribing we need an observable thats why we added of as it is observable.
    }

    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);
    return this.http.get(this.baseUrl + 'account', {headers})
    .pipe(
      map((user: any) => {
        localStorage.setItem('token', user.token);
        this.currentUserSource.next(user);
        this.isAdminSource.next(this.isAdmin(user.token));
      })
    );
  }

  login(value: any): any{
    return this.http.post(this.baseUrl + 'account/login', value)
    .pipe(
      map((user: any) => {
        if (user) {
          localStorage.setItem('token', user.token);
          this.currentUserSource.next(user);
          this.isAdminSource.next(this.isAdmin(user.token));
        }
      })
    );
  }

  register(value: any): any {
    return this.http.post(this.baseUrl + 'account/register', value)
    .pipe(
      map((user: any) => {
        if(user){
          localStorage.setItem('token', user.token);
          this.currentUserSource.next(user);
        }
      })
    );
  }

  logout(): any{
    localStorage.removeItem('token');
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/');
  }

  checkEmailExists(email: string): any {
    return this.http.get(this.baseUrl + 'account/emailexists?email=' + email);
  }

  getUserAddress() {
    return this.http.get<IAddress>(this.baseUrl + 'account/address');
  }

  updateUserAddress(address: IAddress) {
    return this.http.put<IAddress>(this.baseUrl + 'account/address', address);
  }

  isAdmin(token: string): boolean {
    if (token) {
      const decodedToken = JSON.parse(atob(token.split('.')[1]));
      if (decodedToken.role.indexOf('Admin') > -1) {
        return true;
      }
    }
  }


}
