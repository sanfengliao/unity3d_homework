using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour {
    private ParticleSystem ps; //粒子系统(必须)
    private ParticleSystem.Particle[] particles; //粒子系统中的粒子(必须）
    public int count = 1000; //粒子数量
    public float size = 0.3f; //粒子大小，非必须，可在inspector视图中的粒子系统中设置
    public float maxRadius = 12f;//圆环的最大半径
    public float minRadius = 5f;//圆谎的最小半径
    public Gradient gradient; //用来给粒子添加颜色特效
    private float[] radiuses; //粒子的半径
    private float[] angles; //粒子的角度
    // Use this for initialization
    void Start()
    {
        particles = new ParticleSystem.Particle[count];
        radiuses = new float[count];
        angles = new float[count];
        ps = GetComponent<ParticleSystem>();
        ps.maxParticles = count; //设置粒子的数目
        ps.loop = false;//让粒子不循环播放
        ps.startSpeed = 0; //设置粒子的速度
        ps.startSize = size; //设置粒子的数量
        ps.Emit(count); //发射粒子
        ps.GetParticles(particles);
        for (int i = 0; i < count; ++i)
        {   // 随机每个粒子距离中心的半径，同时希望粒子集中在平均半径附近  
            float midRadius = (maxRadius + minRadius) / 2;
            float minRate = Random.Range(1.0f, midRadius / minRadius);
            float maxRate = Random.Range(midRadius / maxRadius, 1.0f);
            float radius = Random.Range(minRadius * minRate, maxRadius * maxRate);

            // 随机每个粒子的角度  
            float angle = Random.Range(0.0f, 360.0f);
            float theta = angle / 180 * Mathf.PI;
            radiuses[i] = radius;
            angles[i] = angle;
            particles[i].position = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0);
        }
        ps.SetParticles(particles, particles.Length);
    }
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < count; ++i)
        {   
            //将粒子的旋转速度分成10个不同的速度，让不同的粒子有不同的速度
            angles[i] -= (i / 100 + 1) / radiuses[i] * Random.Range(1, 3);
            angles[i] = (360.0f + angles[i]) % 360.0f;

            float theta = angles[i] / 180 * Mathf.PI;
            //让粒子的半径在一定范围内波动，产生有利效果
            radiuses[i] += Random.Range(-0.01f, 0.01f);
            particles[i].position = new Vector3(radiuses[i] * Mathf.Cos(theta), radiuses[i] * Mathf.Sin(theta), 0);
            //给粒子加上颜色特效
            particles[i].color = gradient.Evaluate(0.5f);
            
        }
        ps.SetParticles(particles, particles.Length);
	}
}
