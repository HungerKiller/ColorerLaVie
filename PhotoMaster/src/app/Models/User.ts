export class User {
    id: number;
    firstName: string;
    lastName: string;
    username: string;
    password: string;
    role: string;
    token: string;

    constructor(id: number, firstName: string, lastName: string, username: string, password: string, role: string, token: string) {
        this.id = id;
        this.firstName = firstName;
        this.lastName = lastName;
        this.username = username;
        this.password = password;
        this.role = role;
        this.token = token;
    }
}