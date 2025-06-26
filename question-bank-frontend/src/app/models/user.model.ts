export interface User {
  id: number;
  name: string;
  email: string;
  isAdmin: boolean;
}

export interface AddUser {
  name: string;
  email: string;
  password: string;
}

export interface UpdateUser {
  id: number;
  name: string;
  email: string;
  password: string;
}

export interface SearchUser {
  name?: string;
  email?: string;
}
