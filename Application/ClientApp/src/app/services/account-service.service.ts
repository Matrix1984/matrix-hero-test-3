import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Trainer } from '../dtos/user.type';
import { map } from 'rxjs/operators';
import { Registration } from '../dtos/registration.type';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private trainerSubject!: BehaviorSubject<Trainer>;
  public trainer!: Observable<Trainer>;

  constructor(
    private router: Router,
    private http: HttpClient
  ) {

    this.trainerSubject = new BehaviorSubject<Trainer>(JSON.parse(localStorage.getItem('trainer')));
    this.trainer = this.trainerSubject.asObservable();
  }

  public get trainerValue(): Trainer {
    return this.trainerSubject.value;
  }

  login(trainername, password) {
    return this.http.post<Trainer>(`${environment.apiUrl}Auth/Login`, { trainername, password })
      .pipe(map(trainer => {
        localStorage.setItem('trainer', JSON.stringify(trainer));
        this.trainerSubject.next(trainer);
        return trainer;
      }));
  }

  register(user: Registration) {

    return this.http.post(`${environment.apiUrl}Auth/Register`, user);
  }

  logout() {
    localStorage.removeItem('trainer');
    this.trainerSubject.next(null);
    this.router.navigate(['/login']);
  }
}
