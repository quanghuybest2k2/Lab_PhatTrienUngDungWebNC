import { get_api } from './Methods';

export async function getTotalPosts() {
    return get_api(`https://localhost:44309/api/dashboard/posts`);
}

export async function getTotalUnpublishedPosts() {
    return get_api(`https://localhost:44309/api/dashboard/posts/unpublished`);
}

export async function getTotalCategories() {
    return get_api(`https://localhost:44309/api/dashboard/categories`);
}

export async function getTotalAuthor() {
    return get_api(`https://localhost:44309/api/dashboard/authors`);
}

export async function getTotalWaitingComments() {
    return get_api(`https://localhost:44309/api/dashboard/comments/waitingforapprove`);
}

export async function getTotalNewestSubscriber() {
    return get_api(`https://localhost:44309/api/dashboard/subscribers/newer`);
}
export async function getTotalSubscribers() {
    return get_api(`https://localhost:44309/api/dashboard/subscribers`);
}