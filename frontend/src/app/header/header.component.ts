import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { startWith, delay } from 'rxjs/operators';
import { Helpers } from '../helpers/helpers';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit, AfterViewInit {

  subscription: Subscription;
  authentication: boolean;

  constructor(private helpers: Helpers) { }

  ngOnInit() {
  }

  ngAfterViewInit() {
    this.subscription = this.helpers.isAuthenticationChanged().pipe(
      startWith(this.helpers.isAuthenticated()),
      delay(0)).subscribe((value: boolean) =>
        this.authentication = value
      );
  }

  OnDestroy() {
    this.subscription.unsubscribe();
  }

}
