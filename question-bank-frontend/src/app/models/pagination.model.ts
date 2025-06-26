export interface Pagination<T> {
  totalItems: number;
  numberOfItemsPerPage: number;
  numberOfPages: number;
  currentPage: number;
  items: T[];
}
