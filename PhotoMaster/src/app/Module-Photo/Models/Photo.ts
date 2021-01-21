export class Photo {
    Id: number;
    Date: Date;
    Path: string;
    Location: string;
    Description: string;
    Labels: Label[];
}

export class Label {
    Id: number;
    Name: string;
}