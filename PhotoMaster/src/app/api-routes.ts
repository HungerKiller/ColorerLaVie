import { environment } from '../environments/environment';

export class ApiRoute {
    private static readonly BaseUrl = `${environment.host}/${environment.apiPrefix}`;

    public static readonly HOST = `${environment.host}`;

    public static readonly LABEL = {
        getLabels: () => `${ApiRoute.BaseUrl}/labels`,
        getLabel: (labelId: number) => `${ApiRoute.BaseUrl}/labels/${labelId}`,
        postLabel: () => `${ApiRoute.BaseUrl}/labels`,
        putLabel: (labelId: number) => `${ApiRoute.BaseUrl}/labels/${labelId}`,
        deleteLabel: (labelId: number) => `${ApiRoute.BaseUrl}/labels/${labelId}`
    }

    public static readonly PHOTO = {
        getPhotos: () => `${ApiRoute.BaseUrl}/photos`,
        getPhoto: (photoId: number) => `${ApiRoute.BaseUrl}/photos/${photoId}`,
        postPhoto: () => `${ApiRoute.BaseUrl}/photos`,
        putPhoto: (photoId: number) => `${ApiRoute.BaseUrl}/photos/${photoId}`,
        deletePhoto: (photoId: number) => `${ApiRoute.BaseUrl}/photos/${photoId}`,
        getPhotosByLabels: (labelIds: number[]) => {
            let params = '';
            labelIds.map(id => params += `&ids=${id}`);
            return `${ApiRoute.BaseUrl}/photos/bylabels?${params}`;
        },
        uploadPhoto: (photoId: number) => `${ApiRoute.BaseUrl}/photos/upload/${photoId}`
    }
}