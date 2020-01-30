import React, { useEffect } from 'react';
import { Doughnut } from 'react-chartjs-2';
import "./chart_1.css";



const data = {
    labels: ['Donald J. Trump', 'Sachin Rekhi', "Jeff Moser"],
        datasets: [{
        label: 'Number of Fake News',
        data: [30, 2, 3 ],
        backgroundColor: [
            'rgba(255, 99, 132, 0.2)',
            'rgba(54, 162, 235, 0.2)',
            'rgba(255, 206, 86, 0.2)',
        ],
        borderColor: [
            'rgba(255, 99, 132, 1)',
            'rgba(54, 162, 235, 1)',
            'rgba(255, 206, 86, 1)',
        ],
        borderWidth: 1
    }]
};

const Chart_1 = () => {

    return (
        <div>
            <h4 className="gr-title">GRAPHICS</h4>
            <Doughnut data={data} width={300} height={250} />
        </div>
    );
};

export default Chart_1;
