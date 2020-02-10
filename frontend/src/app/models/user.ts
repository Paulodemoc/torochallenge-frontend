import { Stock } from './stock';
import { Account } from './account';

export class User {
  Username: string;
  Password: string;
  Email: string;
  Account: Account;
  Portfolio: Stock[];

  constructor() {
    this.Account = new Account();
  }
}
