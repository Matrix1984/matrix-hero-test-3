import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { AccountService } from 'src/app/services/account-service.service';
import { SubSink } from 'subsink';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  
  form: FormGroup;
  loading = false;
  submitted = false;

  constructor(
      private formBuilder: FormBuilder,
      private route: ActivatedRoute,
      private router: Router,
      private accountService: AccountService,
      private _snackBar: MatSnackBar
  ) { }

  ngOnInit() {
      this.form = this.formBuilder.group({ 
          username: ['', Validators.required],
          email: ['', [Validators.required,Validators.email]], 
          password: ['', [Validators.required, Validators.minLength(8),Validators.pattern('^(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$')]]
      });
  }

  // convenience getter for easy access to form fields
  get f() { return this.form.controls; }

  onSubmit() {
      this.submitted = true; 
      console.info('state of form validation.',this.form.invalid);
      // stop here if form is invalid
      if (this.form.invalid) {
          return;
      }
  
      this.loading = true;

      this.accountService.register(this.form.value)
          .pipe(first())
          .subscribe({
              next: () => { 
                  this.router.navigate([''], { relativeTo: this.route });
              },
              error: error => { 
                  this._snackBar.open(error.message, 'Close');
                  console.error(error);
                  this.loading = false;
              }
          });
  }
}
 