import axios, { AxiosError } from 'axios'

export function parseAxiosError( error: unknown ): string[] {
    let retValue: string[] = [];

    if (axios.isAxiosError(error)) {
        const err = error as AxiosError

        if(err.response?.data) {
            if(Array.isArray( err.response?.data )) {
                retValue = err.response.data
            }
            else {
                const { detail, status, title } = err.response.data

                retValue = [`${err.message}: ${title} - ${detail}`]
            }

        }
        else {
            retValue = [err.message]
        }
    }

    return retValue;
}