import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TokenService } from '../services/token.service';
import { Helpers } from '../helpers/helpers';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  username = '';
  password = '';

  constructor(private helpers: Helpers, private router: Router, private tokenService: TokenService) { }

  ngOnInit() {
  }

  login(): void {
    this.tokenService.auth({Username: this.username, Password: this.password}).subscribe((token: any) => {
      this.helpers.setToken(token);
      this.router.navigate(['/dashboard']);
    });
  }
}
