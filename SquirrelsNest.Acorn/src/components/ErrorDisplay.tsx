interface displayErrorsProps{
    errors?: string[];
}
export default function ErrorDisplay(props: displayErrorsProps){
    const style = {color: 'red'};
    return (
        <>
            {props.errors ? <ul style={style}>
                {props.errors.map((error, index) => <li key={index}>{error}</li>)}
            </ul>: null}
        </>
    )
}
