import { get_api } from './Methods';

export function getComments() {
    return get_api(`https://localhost:44309/api/comments?Censored=true&PageSize=10&PageNumber=1`);
}
export async function getCommentById(id = 0) {
    if (id > 0) return get_api(`https://localhost:44309/api/comments/${id}`);
    return null;
}

