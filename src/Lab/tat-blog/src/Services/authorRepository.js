import { get_api } from './Methods';

export async function getBestAuthor(limit = 5) {
    return get_api(`https://localhost:44309/api/authors/best/${limit}`);
}

export function getAuthors() {
    return get_api(`https://localhost:44309/api/authors?PageSize=10&PageNumber=1`);
}
export async function getAuthorById(id = 0) {
    if (id > 0) {
        return get_api(`https://localhost:44309/api/authors/${id}`);
    } else {
        return null;
    }
}

