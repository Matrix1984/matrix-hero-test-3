import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router'; 
import { AppComponent } from './app.component';
import { HeroesMainComponent } from './heroes-main/heroes-main.component';
import { LoginComponent } from './auth/login/login.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthGuard } from './auth/guards/auth-guard.service';
import { ErrorInterceptor } from './interceptors/error-interceptor.service';
import { JwtInterceptor } from './interceptors/jwt-interceptor.service';
import { CommonModule } from '@angular/common';
import { RegisterComponent } from './auth/register/register.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import {MatButtonModule} from '@angular/material/button';
import {MatTableModule} from '@angular/material/table';
import {MatSnackBarModule} from '@angular/material/snack-bar';


@NgModule({
  declarations: [
    AppComponent, 
    HeroesMainComponent,
    RegisterComponent,
    LoginComponent,
    PageNotFoundComponent 
  ],
  imports: [
    CommonModule,
    BrowserModule,//.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule, 
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatTableModule,
    MatSnackBarModule,
    RouterModule.forRoot([
     { path: '', component: HeroesMainComponent, pathMatch: 'full',canActivate: [AuthGuard] }, 
     { path: 'login', component: LoginComponent }, 
     { path: 'register', component: RegisterComponent }, 
     { path: '**', component: PageNotFoundComponent }
    ]),
    BrowserAnimationsModule
  ],
  providers: [
     { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
     { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

  