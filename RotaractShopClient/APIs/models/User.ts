import {Role} from './Role';

export class User {
  UserId ?: number;
  FirstName ?: string;
  LastName ?: string;
  DistrictNumber ?: number;
  Username ?: string;
  Email ?: string;
  Password ?: string;
  Role ?: Role;
}
