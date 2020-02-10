import { Component, OnInit, AfterViewInit } from '@angular/core';
import { User } from '../models/user';
import { Subscription } from 'rxjs';
import { Helpers } from '../helpers/helpers';
import { startWith, delay } from 'rxjs/operators';
import { UsersService } from '../services/users.service';
import { Account } from '../models/account';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-funds',
  templateUrl: './funds.component.html',
  styleUrls: ['./funds.component.css']
})
export class FundsComponent implements OnInit, AfterViewInit {
  userData: User;
  subscription: Subscription;
  authentication: boolean;

  constructor(private helpers: Helpers, private userService: UsersService, private toastr: ToastrService) { }

  ngOnInit() {
    this.userData = new User();
    this.userData.Account = new Account();
  }

  ngAfterViewInit() {
    this.subscription = this.helpers.isAuthenticationChanged().pipe(
      startWith(this.helpers.isAuthenticated()),
      delay(0)).subscribe((value: boolean) => {
        this.authentication = value;
        if (value) {
          this.getUserData();
        }
      });
  }

  private getUserData() {
    this.userService.getUserData().subscribe(user => {
      this.userData = user;
      if (this.userData.Account === null) {
        this.userData.Account = new Account();
        this.userData.Account.Funds = 0;
      }
    });
  }

  OnDestroy() {
    this.subscription.unsubscribe();
  }

  withdraw() {
    if(this.userData.Account.Ammount === undefined) {
      this.toastr.warning('Please inform the ammount you want to withdraw', 'Attention');
      return;
    }

    this.userService.removeFunds(this.userData.Account.Ammount).subscribe((data) => {
      this.toastr.success('Funds removed');
      this.getUserData();
    }, (error) => {
      this.toastr.error(error.error);
    });
  }

  deposit() {
    if(this.userData.Account.Ammount === undefined) {
      this.toastr.warning('Please inform the ammount you want to deposit', 'Attention');
      return;
    }

    this.userService.addFunds(this.userData.Account.Ammount).subscribe((data) => {
      this.toastr.success('Funds added');
      this.getUserData();
    }, (error) => {
      this.toastr.error(error.error);
    });
  }

}
