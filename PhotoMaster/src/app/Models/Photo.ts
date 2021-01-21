import { Label } from './Label';

export class Photo {
    Id: number;
    Date: Date;
    Path: string;
    Location: string;
    Description: string;
    Labels: Label[];

    constructor(id: number, date: Date, location: string, description: string, labels: Label[]){
        this.Id = id;
        this.Date = date;
        this.Location = location;
        this.Description = description;
        this.Labels = labels;
    }
}