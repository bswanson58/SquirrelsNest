import {HttpClient} from '@angular/common/http'
import {Injectable} from '@angular/core'
import {Observable} from 'rxjs'
import {ClProject} from '../Data/graphQlTypes'

@Injectable( {
  providedIn: 'root'
} )
export class ProjectTransferService {
  constructor( private http: HttpClient ) {
  }

  DownloadProject( project: ClProject ): Observable<Blob> {
    return this.http.get( `https://localhost:8200/transfer/export?projectId=${project.id}`, { responseType: 'blob' } )
  }
}
