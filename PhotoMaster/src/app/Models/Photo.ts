import { Label } from './Label';

export class Photo {
    id: number;
    date: string;
    path: string;
    location: string;
    description: string;
    labels: Label[];

    constructor(id: number, date: string, location: string, description: string, labels: Label[]){
        this.id = id;
        this.date = date;
        this.location = location;
        this.description = description;
        this.labels = labels;
    }
}