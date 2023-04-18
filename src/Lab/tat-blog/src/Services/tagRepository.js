import { get_api } from './Methods';

export function getTags() {
    return get_api(`https://localhost:44309/api/tags?PageSize=10&PageNumber=1`);
}

export async function getTagById(id = 0) {
    if (id > 0) {
        return get_api(`https://localhost:44309/api/tags/${id}`);
    } else {
        return null;
    }
}
