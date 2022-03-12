import { HttpClient } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { SubSink } from 'subsink';
import { Hero } from '../dtos/hero.type';
import { Trainer } from '../dtos/user.type';
import { AccountService } from '../services/account-service.service';

@Component({
  selector: 'app-heroes-main',
  templateUrl: './heroes-main.component.html',
  styleUrls: ['./heroes-main.component.css']
})
export class HeroesMainComponent implements OnInit,OnDestroy  {

 // private subs = new SubSink();
  
  trainer: Trainer;

  dataSource: Hero[];
 
  displayedColumns=['heroId','guidId','heroTrainingDate','name',
                    'colors','startPower','currentPower','actions'];

  constructor(private router: Router,
    private accountService: AccountService,
    private http: HttpClient) { }

  ngOnInit(): void {
    this.accountService.trainer.subscribe(x => {
      this.trainer = x
      this.loadHeroes();
    }); 
  }

 

  trainHero(heroId: number){
     this.http.post(`${environment.apiUrl}Heroes`,{ heroId: heroId}).subscribe(res=>{
      console.log(res);
    },
    err=>{
      console.error(err)
    });
  }

  loadHeroes(){
    this.http.get<Hero[]>(`${environment.apiUrl}Heroes`).subscribe(res=>{
      console.log(res);
      this.dataSource=res;
    },
    err=>{
      console.error(err)
    });
  }

  logout() {
    this.accountService.logout();
  }

  ngOnDestroy() {
  //  this.subs.unsubscribe();
  }
}
 