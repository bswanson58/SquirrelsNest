import {Injectable} from '@angular/core'
import {BehaviorSubject, Observable} from 'rxjs'
import {StateCategory} from '../Data/graphQlTypes'

export interface CategoryValues {
  name: string,
  value: string
}

const categoryValues: CategoryValues[] = [
  { name: 'Initial', value: 'INITIAL' as StateCategory.Initial },
  { name: 'Intermediate', value: 'INTERMEDIATE' as StateCategory.Intermediate },
  { name: 'Completed', value: 'COMPLETED' as StateCategory.Completed },
  { name: 'Terminal', value: "TERMINAL" as StateCategory.Terminal }
]

const categoryObservable = new BehaviorSubject<CategoryValues[]>( categoryValues )

@Injectable( {
  providedIn: 'root'
} )
export class ProjectConstants {
  GetWorkflowStateCategoryValues(): CategoryValues[] {
    return categoryValues
  }

  GetWorkflowStateCategoryValues$(): Observable<CategoryValues[]> {
    return categoryObservable
  }
}
