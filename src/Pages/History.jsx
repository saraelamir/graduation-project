import React from 'react';
import { PieChart as RechartsPieChart, Pie, Cell } from 'recharts';
import styles from './history.module.css'; // استيراد CSS Module
import HeaderBar from "../components/HeaderBar";

function HistoryPage() {
  const plans = [
    {
      id: 1,
      date: '1/3/2023',
      expenses: {
        housing: 40,
        entertainment: 25,
        others: 20,
        food: 15,
        transportation: 10,
      },
    },
    {
      id: 2,
      date: '2/2/2024',
      expenses: {
        housing: 35,
        entertainment: 30,
        others: 20,
        food: 25,
        transportation: 35,
      },
    },
    {
      id: 3,
      date: '1/5/2025',
      expenses: {
        housing: 30,
        entertainment: 10,
        others: 10,
        food: 15,
        transportation: 40,
      },
    },
  ];

  const COLORS = ['#87CEFA', '#1E90FF', '#00CED1', '#5F9EA0', '#3DB8E0'];

  return (
    <div className={styles.historyPage}>
      <div className={styles.header}>
        <h1>History Page</h1> <HeaderBar />


      </div>

      <div className={styles.plansContainer}>
        {plans.map((plan) => (
          <div className={styles.planRow} key={plan.id}>
            <div className={styles.square}>
              <h3>Plan {plan.id}</h3>
              <p>{plan.date}</p>
            </div>

            <div className={styles.rectangle}>
              <div className={styles.chartAndDetails}>
                <div className={styles.pieChartContainer}>
                  <RechartsPieChart width={250} height={250}>
                    <Pie
                      data={Object.entries(plan.expenses).map(([name, value]) => ({ name, value }))}
                      dataKey="value"
                      nameKey="name"
                      cx="50%"
                      cy="50%"
                      outerRadius={80}
                      labelLine={false}
                      label={({ name, value, cx, cy, midAngle, outerRadius }) => {
                        const RADIAN = Math.PI / 180;
                        const radius = outerRadius - 20;
                        const x = cx + radius * Math.cos(-midAngle * RADIAN);
                        const y = cy + radius * Math.sin(-midAngle * RADIAN);
                        return (
                          <text
                            x={x}
                            y={y}
                            textAnchor="middle"
                            dominantBaseline="middle"
                            fill="black"
                            fontSize={12}
                            fontWeight="bold"
                          >
                            {`${value}%`} {/* إبقاء النسبة داخل الباي تشارت */}
                          </text>
                        );
                      }}
                    >
                      {Object.entries(plan.expenses).map((entry, index) => (
                        <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                      ))}
                    </Pie>
                  </RechartsPieChart>
                </div>

                <div className={styles.expenseDetails}>
                  {Object.entries(plan.expenses).map(([name, value], index) => (
                    <div className={styles.expenseItem} key={name}>
                      <span
                        className={styles.colorDot}
                        style={{ backgroundColor: COLORS[index % COLORS.length] }}
                      ></span>
                      <span>{name}</span> {/* إظهار اسم الفئة فقط دون النسبة */}
                    </div>
                  ))}
                </div>

              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}

export default HistoryPage;
