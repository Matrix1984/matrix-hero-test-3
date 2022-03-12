import { Component } from '@angular/core';
import { Trainer } from './dtos/user.type';
import { AccountService } from './services/account-service.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  user: Trainer;

  constructor(private accountService: AccountService) {
      this.accountService.trainer.subscribe(x => this.user = x);
  }

  logout() {
      this.accountService.logout();
  }
}
